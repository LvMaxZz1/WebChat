<template>
  <div>
    <div class="chat-pad">
      <div class="chat-item" v-for="chat in chats">
        <div v-if="chat.role == 'user'" style="overflow: hidden">
          <p class="user-chat">{{ chat.content }}</p>
        </div>
        <div v-else-if="chat.role == 'assistant'" style="overflow: hidden">
          <p class="assistant-chat">
            {{ chat.content }}
          </p>
        </div>
      </div>
      <textarea v-model="input" name="" id="" rows="6"></textarea>
      <button @click="onSend" :disabled="!canClickSend">发送</button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'

const input = ref('')
const canSend = ref(true)

const canClickSend = computed(() => input.value.trim() !== '' && canSend.value)

const chats = ref<any>([])

function onSend() {
  canSend.value = false
  let baseUrl = 'http://localhost:5262/'
  let url = baseUrl + 'api/ai'
  chats.value.push({
    role: 'user',
    content: input.value,
  })
  input.value = ''
  const response = fetch(url, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ ChatItemDtos: chats.value }),
  })
  canSend.value = true
}

onMounted(() => {
  chats.value.push({
    role: 'system',
    content: '你是一个诗人',
  })
})
</script>

<style lang="scss" scoped>
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
</style>
