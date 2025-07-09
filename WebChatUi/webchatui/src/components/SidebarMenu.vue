<template>
  <div class="sidebar-menu">
    <div class="logo">WebChat</div>
    <button class="new-chat">+ New Chat</button>
    <div class="menu-list">
      <!-- 假数据菜单项 -->
      <div v-for="item in menuItems" :key="item.id" class="menu-item">
        <span>{{ item.label }}</span>
      </div>
    </div>
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
</script>

<style scoped>
.sidebar-menu {
  width: 80px;
  background: #2563eb;
  color: #fff;
  display: flex;
  flex-direction: column;
  align-items: center;
  padding-top: 16px;
}

.logo {
  font-size: 20px;
  font-weight: bold;
  margin-bottom: 24px;
}

.new-chat {
  width: 64px;
  height: 36px;
  background: #fff;
  color: #2563eb;
  border: none;
  border-radius: 8px;
  margin-bottom: 24px;
  cursor: pointer;
  font-weight: bold;
}

.menu-list {
  flex: 1;
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.menu-item {
  width: 100%;
  text-align: center;
  padding: 12px 0;
  cursor: pointer;
  border-radius: 6px;
  transition: background 0.2s;
}

.menu-item:hover {
  background: rgba(255, 255, 255, 0.1);
}
</style>
