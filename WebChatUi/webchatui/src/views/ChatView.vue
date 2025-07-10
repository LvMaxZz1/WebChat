<template>
  <div class="common-layout">
    <el-container>
      <el-container>
        <el-aside width="310">
          <ChatList />
        </el-aside>
        <el-main class="el-main-class">
          <ChatPanel/>
        </el-main>
      </el-container>
    </el-container>
  </div>
</template>
<script setup lang="ts">
import ChatList from '../components/ChatList.vue'
import ChatPanel from '../components/ChatPanel.vue'
</script>

<style scoped lang="scss">
.el-main-class{
  height: 80vw;
}
</style>
