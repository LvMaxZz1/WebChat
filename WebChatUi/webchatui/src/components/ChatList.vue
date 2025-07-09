<template>
  <div class="chat-list">
    <el-container class="chat-list-container">
      <el-header class="header-center">
        <el-tooltip content="新建对话" placement="top">
          <button class="new-chat">+ New Chat</button>
        </el-tooltip>
      </el-header>
      <el-divider></el-divider>
      <el-aside>
        <div
          v-for="item in menuItems"
          :key="item.id"
          :class="['chat-item', item.id == selectedId ? 'active' : '']"
          @click="selectChat(item.id)"
        >
          <div class="chat-info">
            <div class="chat-title">{{ item.label }}</div>
          </div>
        </div>
      </el-aside>
    </el-container>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const menuItems = ref<any>([
  { id: 1, label: 'Getting Started' },
  { id: 2, label: 'Product Features' },
  { id: 3, label: 'Technical Support' },
  { id: 4, label: 'Pricing Plans' },
  { id: 5, label: 'Integration Help' },
])

const selectedId = ref(1)

function selectChat(id: number) {
  selectedId.value = id
}
</script>

<style scoped lang="scss">

.header-center{
  display: flex;
  justify-content: center;
  align-items: center;
}

.chat-list-container{
  height: 100%;
  width: 300px;
  border: 1px solid var(--el-border-color, #e5e7eb);
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

.chat-item.active,
.chat-item:hover {
  background: #e8edfa;
}

.chat-avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  margin-right: 12px;
}

.chat-info {
  flex: 1;
}

.chat-title {
  font-weight: bold;
  font-size: 16px;
  margin-bottom: 4px;
}

.chat-desc {
  font-size: 13px;
  color: #888;
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
  margin-top: 25px;
}
</style>
