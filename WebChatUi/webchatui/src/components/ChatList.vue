<template>
  <div class="chat-list">
    <el-container class="chat-list-container">
      <el-header class="header-center">
        <el-tooltip content="新建对话" placement="top">
          <button class="new-chat" @click="newChatClick">+ New Chat</button>
        </el-tooltip>
      </el-header>
      <el-divider></el-divider>
      <el-scrollbar ref="scrollbarRef">
        <el-aside>
          <div
            v-for="item in chatStore.menuItems"
            :key="item.id"
            :class="['chat-item', item.id == chatStore.selectedId ? 'active' : '']"
            @click="chatStore.selectChat(item.id)"
          >
            <div class="chat-info">
              <div class="chat-title">{{ item.label }}</div>
              <el-button link type="info" @click="deleteChatClick(item.id)"  style="padding: 0 6px">
                <el-icon class="el-icon-class">
                  <CircleClose />
                </el-icon>
              </el-button>
            </div>
          </div>
        </el-aside>
      </el-scrollbar>
    </el-container>
  </div>
</template>

<script setup lang="ts">
import { useChatStore } from '@/stores/chatStore'
import type { ElScrollbar } from 'element-plus'
import { ref } from 'vue'
import { CircleClose } from '@element-plus/icons-vue'

const chatStore = useChatStore()
const scrollbarRef = ref<InstanceType<typeof ElScrollbar> | null>(null)

function newChatClick() {
  chatStore.newChat()
  chatStore.rollTick(scrollbarRef)
}

function deleteChatClick(id:number) {
  chatStore.deleteChat(id)
}

</script>

<style scoped lang="scss">
.header-center {
  display: flex;
  justify-content: center;
  align-items: center;
}

.chat-list-container {
  height: 100%;
  width: 300px;
}

.chat-list {
  background: #f7f8fa;
  overflow-y: auto;
  padding-top: 16px;
  height: calc(100vh - 34px);
  border-radius: 2px;
}

.chat-item {
  display: flex;
  align-items: center;
  padding: 12px 16px;
  cursor: pointer;
  border-radius: 8px;
  margin-bottom: 8px;
  transition: background 0.2s;
}

.chat-item:hover {
  background: #e8edfa;
}

.chat-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex: 1;
}

.chat-title {
  font-weight: bold;
  font-size: 16px;
  margin-bottom: 4px;
}

.new-chat {
  width: 200px;
  height: 50px;
  background: #2563eb;
  color: #fff;
  border: 1px solid var(--el-border-color, #e5e7eb);
  border-radius: 8px;
  cursor: pointer;
  font-weight: bold;
  margin-bottom: 10px;
}

.el-icon-class{
  color: #42b8dd;
}
</style>
