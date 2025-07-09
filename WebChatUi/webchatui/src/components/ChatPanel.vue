<template>
  <div class="chat-panel">
    <div class="chat-header">
      <div class="chat-title">WebChat</div>
      <div class="chat-status">已连接</div>
    </div>
    <div class="chat-messages">
      <div v-for="msg in messages" :key="msg.id" :class="['chat-message', msg.role]">
        <div class="msg-bubble">
          <div class="msg-content">{{ msg.content }}</div>
        </div>
        <div class="msg-time">{{ msg.time }}</div>
      </div>
    </div>
    <div class="chat-input-bar">
      <input class="chat-input" type="text" placeholder="输入消息..." />
      <button class="chat-send-btn">
        <svg width="20" height="20" fill="#2563eb" viewBox="0 0 24 24"><path d="M2 21l21-9-21-9v7l15 2-15 2z"/></svg>
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
const messages = [
  { id: 1, role: 'ai', content: '您好！我是 WebChat AI 助手，很高兴为您服务。', time: '10:30 AM' },
  { id: 2, role: 'user', content: '你好！我想了解一下你们的产品功能', time: '10:31 AM' },
  { id: 3, role: 'ai', content: 'WebChat 提供以下主要功能：\n- 智能对话与问答\n- 多语言实时翻译\n- 知识库查询\n- 个性化推荐\n- 24/7 全天候服务', time: '10:32 AM' },
];
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
  border-radius:20px;
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
  flex: 1;
  padding: 24px;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
  gap: 16px;
}
.chat-message {
  display: flex;
  flex-direction: column;
  align-items: flex-start;
}
.chat-message.user {
  align-items: flex-end;
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
.chat-message.user .msg-bubble {
  background: #2563eb;
  color: #fff;
}
.msg-time {
  font-size: 12px;
  color: #aaa;
  margin-left: 8px;
}
.chat-input-bar {
  display: flex;
  align-items: center;
  padding: 16px 24px;
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
</style>
