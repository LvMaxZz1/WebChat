<template>
  <div class="common-layout">
    <el-container>
      <el-container>
        <el-aside width="310">
          <ChatList />
        </el-aside>
        <el-main class="el-main-class">
          <ChatPanel
            v-if="chatStore.menuItems.length > 0 && chatStore.menuItems.find(item => item.id === chatStore.selectedId)"
          />
          <SelectOrCreateChatTip v-else />
        </el-main>
      </el-container>
    </el-container>
  </div>
</template>
<script setup lang="ts">
import ChatList from '../components/ChatList.vue'
import ChatPanel from '../components/ChatPanel.vue'
import { useChatStore } from '@/stores/chatStore'
import SelectOrCreateChatTip from '@/components/SelectOrCreateChatTip.vue'

const chatStore = useChatStore()

</script>

<style scoped lang="scss">
.el-main-class{
  height: 80vw;
}
</style>
