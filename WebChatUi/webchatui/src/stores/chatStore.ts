import { defineStore } from 'pinia'
import { nextTick } from 'vue'
import type { ChatInterface } from '@/stores/interfaces/ChatInterface.ts'

export const useChatStore = defineStore('chat', {
  state: () => ({
    menuItems: [] as Array<{ id: number; label: string }>,
    selectedId:  0 as number,
    chatsMap: {} as Record<number, ChatInterface[]>
  }),
  actions: {
    selectChat(id: number) {
      this.selectedId = id
    },
    newChat() {
      const newId = Date.now()
      this.menuItems.push({ id: newId, label: `新会话${this.menuItems.length + 1}` })
      this.chatsMap[newId] = []
      this.selectedId = newId
    },
    updateChats(chats: ChatInterface[]) {
      this.chatsMap[this.selectedId] = chats
    },
    deleteChat(id: number) {
      this.menuItems = this.menuItems.filter(item => item.id !== id)
      delete this.chatsMap[id]
      // 如果删除的是当前选中的会话
      if (this.selectedId === id) {
        // 新建一个空会话并切换到它
        this.newChat()
      }
    },
    async rollTick(scrollbarRef) {
      await nextTick(() => {
        if (scrollbarRef.value) {
          scrollbarRef.value.setScrollTop(Number.MAX_SAFE_INTEGER)
        }
      })
    }
  }
})
