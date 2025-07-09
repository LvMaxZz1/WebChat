<template>
  <div class="chat-pad-container">
    <div class="chat-pad">
      <div class="chat-item" v-for="(chat, index) in chats" :key="index">
        <div v-if="chat.role == 'user'" style="overflow: hidden">
          <p class="user-chat">{{ chat.content }}</p>
        </div>
        <div v-else-if="chat.role == 'assistant'" style="overflow: hidden">
          <p class="assistant-chat">
            <template v-for="(part, idx) in parseAssistantContent(chat.content)" :key="idx">
              <span v-if="part.type === 'text'">{{ part.value }}</span>
              <span v-else class="think-box">{{ part.value }}</span>
            </template>
          </p>
        </div>
      </div>
      <textarea v-model="input"  rows="6"></textarea>
      <button @click="onSend" :disabled="!canClickSend">发送</button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import type { ChatInterface } from '../stores/interfaces/ChatInterface'

const input = ref('')
const canSend = ref(true)

const canClickSend = computed(() => input.value.trim() !== '' && canSend.value)

const chats = ref<ChatInterface[]>([])

async function onSend() {
  canSend.value = false
  const baseUrl = 'http://localhost:5262/'
  const url = baseUrl + 'api/ai'
  chats.value.push({
    role: 'user',
    content: input.value
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
    content: ''
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
    }
  }
}

// 解析思考内容和回复内容
function parseAssistantContent(content: string) {
  // 匹配 <think>...</think>
  const thinkRegex = /<think>([\s\S]*?)<\/think>/g;
  const result: Array<{ type: 'think' | 'text', value: string }> = [];
  let lastIndex = 0;
  let match: RegExpExecArray | null;

  while ((match = thinkRegex.exec(content)) !== null) {
    // 普通文本
    if (match.index > lastIndex) {
      result.push({
        type: 'text',
        value: content.slice(lastIndex, match.index)
      });
    }
    // 思考内容
    result.push({
      type: 'think',
      value: match[1].trim()
    });
    lastIndex = thinkRegex.lastIndex;
  }
  // 剩余文本
  if (lastIndex < content.length) {
    result.push({
      type: 'text',
      value: content.slice(lastIndex)
    });
  }
  return result;
}

onMounted(() => {
  chats.value.push({
    role: 'system',
    content: '你是一个聊天好伙伴，你能够陪用户聊天'
  })
})
</script>

<style lang="scss" scoped>
.chat-pad-container{
    margin-left: 50px;
}

.chat-pad {
  width: 600px;
  margin: auto;
}

.user-chat {
  background-color: skyblue;
  float: right;
  color: black;
  border-radius: 4xp;
  padding: 10px;
}

.assistant-chat {
  background-color: #ccc;
  float: left;
  color: black;
  border-radius: 4xp;
  padding: 10px;
}

textarea {
  width: 100%;
  resize: none;
  margin-top: 20px;
}

.think-box {
  display: block;
  background: #f5f7fa;
  border-left: 4px solid #409eff;
  color: #333;
  margin: 10px 0;
  padding: 10px 16px;
  border-radius: 4px;
  font-style: italic;
  font-size: 0.95em;
  box-shadow: 0 2px 8px rgba(64,158,255,0.08);
}
</style>
