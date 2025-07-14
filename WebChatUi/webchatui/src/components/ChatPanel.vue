<template>
  <div class="chat-panel">
    <div class="chat-header">
      <div class="chat-title">WebChat</div>
      <div class="chat-status">已连接</div>
    </div>
    <div class="chat-messages">
      <el-scrollbar ref="scrollbarRef">
        <div v-for="(chat, index) in chats" :key="index">
          <div class="chat-message">
            <div class="msg-bubble" v-if="chat.role === 'assistant'">
              <template v-for="(part, idx) in parseAssistantContent(chat.content)" :key="idx">
                <div v-if="part.type === 'text'">
                  <el-text>{{ part.value }}</el-text>
                </div>
                <div v-else>
                  <el-button @click="updateThinkExpanded(chat)"  class="el-button-expanded" link type="info" style="padding: 0 4px;">
                    思考过程
                    <el-icon class="el-icon-class">
                      <ArrowUp v-if="chat.thinkExpanded" />
                      <ArrowDown v-else />
                    </el-icon>
                  </el-button>
                  <el-text
                    class="think-box"
                    v-show="chat.thinkExpanded">
                    {{ part.value }}
                  </el-text>
                </div>
              </template>
              <div>
                <el-text class="msg-time">{{ chat.time }}</el-text>
              </div>
            </div>
          </div>
          <div class="chat-message-user">
            <div class="msg-bubble" v-if="chat.role === 'user'">
              <div>
                <el-text >{{ chat.content }}</el-text>
              </div>
              <div>
                <el-text class="msg-time">{{ chat.time }}</el-text>
              </div>
            </div>
          </div>
        </div>
      </el-scrollbar>
      <div class="chat-input-bar">
        <input v-model="input" class="chat-input" type="text" placeholder="输入消息..." />
        <button class="chat-send-btn" @click="onSend" :disabled="!canClickSend">
          <svg width="20" height="20" fill="#2563eb" viewBox="0 0 24 24">
            <path d="M2 21l21-9-21-9v7l15 2-15 2z" />
          </svg>
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue'
import type { ChatInterface } from '@/stores/interfaces/ChatInterface.ts'
import type { ElScrollbar } from 'element-plus'
import { ArrowUp, ArrowDown } from '@element-plus/icons-vue'
import { useChatStore } from '@/stores/chatStore.ts'

const chatStore = useChatStore()
const chats = ref([...chatStore.chatsMap[chatStore.selectedId] || []])

const input = ref('')
const canSend = ref(true)
const canClickSend = computed(() => input.value.trim() !== '' && canSend.value)
const scrollbarRef = ref<InstanceType<typeof ElScrollbar> | null>(null)

// 监听切换会话
watch(() => chatStore.selectedId, (id) => {
  chats.value = [...chatStore.chatsMap[id] || []]
})

// 监听内容变化，自动同步到 store
watch(chats, (val) => {
  chatStore.updateChats(val)
}, { deep: true })

async function onSend() {
  canSend.value = false
  const baseUrl = 'http://localhost:5262/'
  const url = baseUrl + 'api/ai'
  chats.value.push({
    role: 'user',
    content: input.value,
    time: new Date().toDateString(),
    thinkExpanded:true
  })
  input.value = ''
  const response = await fetch(url, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ ChatItemDtos: chats.value })
  })
  canSend.value = true
  let result = null
  chats.value.push({
    role: 'assistant',
    content: '',
    time: new Date().toDateString(),
    thinkExpanded:true
  })
  const assistantMessageIndex = chats.value.length - 1

  if (response.ok && response.body != null) {
    result = response.body.getReader()
    const decoder = new TextDecoder('utf-8')
    while (true) {
      if (result == null) return
      const { done, value } = await result.read()
      if (done) break
      let text = decoder.decode(value, { stream: true })
      if (text.startsWith('[')|| text.startsWith(',')){
        text = text.substring(1, text.length)
        const jObject = JSON.parse(text)
        chats.value[assistantMessageIndex].content += jObject.contents[0].text
      }
       await chatStore.rollTick(scrollbarRef)
    }
  }

  // 测试逻辑
  // for (let i = 0; i < 200; i++) {
  //   if (i == 0) chats.value[assistantMessageIndex].content += '<think>'
  //   else if (i == 99) chats.value[assistantMessageIndex].content += '</think>'
  //   else chats.value[assistantMessageIndex].content += i
  //   await new Promise((resolve) => setTimeout(resolve, 10))
  //   await chatStore.rollTick(scrollbarRef)
  // }
  chats.value[assistantMessageIndex].thinkExpanded = false
}

// 解析思考内容和回复内容
function parseAssistantContent(content: string) {
  // 匹配 <think>...</think>
  const thinkRegex = /<think>([\s\S]*?)<\/think>/g
  const result: Array<{ type: 'think' | 'text'; value: string }> = []
  let lastIndex = 0
  let match: RegExpExecArray | null

  while ((match = thinkRegex.exec(content)) !== null) {
    // 普通文本
    if (match.index > lastIndex) {
      result.push({
        type: 'text',
        value: content.slice(lastIndex, match.index),
      })
    }
    // 思考内容
    result.push({
      type: 'think',
      value: match[1].trim(),
    })
    lastIndex = thinkRegex.lastIndex
  }
  // 剩余文本
  if (lastIndex < content.length) {
    result.push({
      type: 'text',
      value: content.slice(lastIndex),
    })
  }
  return result
}

function updateThinkExpanded(chat: ChatInterface){
  chat.thinkExpanded = !chat.thinkExpanded
}

onMounted(() => {
  chats.value.push({
    role: 'system',
    content: '你是一个聊天好伙伴，你能够陪用户聊天',
    time: new Date().toDateString(),
    thinkExpanded: true
  })
})
</script>

<style scoped>
.chat-panel {
  flex: 1;
  display: flex;
  flex-direction: column;
  background: #fafbfc;
  height: 100%;
  width: 100%;
}
.chat-header {
  height: 56px;
  background: #fff;
  border: 1px solid var(--el-border-color, #e5e7eb);
  border-radius: 20px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 24px;
}
.chat-title {
  font-size: 20px;
  font-weight: bold;
  color: #111827;
}
.chat-status {
  color: #22c55e;
  font-size: 14px;
}
.chat-messages {
  height: 85vh; /* 固定高度 */
  overflow-y: auto;
  padding: 24px;
  display: flex;
  flex-direction: column;
  gap: 16px;
}
.chat-message {
  display: flex;
  flex-direction: column;
  align-items: flex-start;
}
.chat-message-user {
  align-items: flex-end;
  display: flex;
  flex-direction: column;
}
.msg-bubble {
  max-width: 60%;
  padding: 12px 16px;
  border-radius: 12px;
  background: #e8edfa;
  color: #222;
  font-size: 15px;
  margin-bottom: 4px;
  white-space: pre-line;
}
.chat-message-user .msg-bubble {
  background: #e7f8ff;
  color: #fff;
  margin-right: 20px;
}
.msg-time {
  font-size: 12px;
  color: #aaa;
  margin-left: 8px;
}
.chat-input-bar {
  display: flex;
  align-items: center;
  padding: 16px 12px;
  background: #fff;
  border-top: 1px solid #e5e7eb;
}
.chat-input {
  flex: 1;
  padding: 10px 16px;
  border-radius: 8px;
  border: 1px solid #e5e7eb;
  font-size: 15px;
  outline: none;
  margin-right: 12px;
}
.chat-send-btn {
  background: none;
  border: none;
  cursor: pointer;
  padding: 6px;
  border-radius: 6px;
  transition: background 0.2s;
}
.chat-send-btn:hover {
  background: #e8edfa;
}
.think-box {
  display: block;
  background: #f5f7fa;
  color: #ccc;
  margin-bottom: 20px;
  padding: 10px 16px;
  border-radius: 10px;
  font-style: italic;
  font-size: 0.95em;
  box-shadow: 0 2px 8px rgba(64, 158, 255, 0.08);
  border-top: 1px solid var(--el-border-color);
}
.el-icon-class{
  margin-left: 5px;
}
.el-button-expanded{
  margin-bottom: 10px;
}
</style>
