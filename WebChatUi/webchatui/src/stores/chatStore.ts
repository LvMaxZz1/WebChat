import { defineStore } from 'pinia'
import { nextTick } from 'vue'

export const useChatStore = defineStore('chat', {
  state: () => ({
    menuItems: [
      { id: 1, label: '新会话1' }
    ],
    selectedId: 1,
    chatsMap: {
      1: []
    } as Record<number, any[]>
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
    updateChats(chats: any[]) {
      this.chatsMap[this.selectedId] = chats
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
