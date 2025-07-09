<template>
  <div>
    <RouterView />
  </div>
</template>

<script setup lang="ts">
import {ref, onMounted} from 'vue'

const inputColor=ref('pink')
onMounted(()=>{
  let glColor = localStorage.getItem('glColor')
  if(glColor){
    document.documentElement.style.setProperty('--gl-color', glColor)
  }
})
const changeColor = () => {
  document.documentElement.style.setProperty('--gl-color', inputColor.value)
  localStorage.setItem('glColor', inputColor.value)
}


</script>

<style scoped lang="scss">
:root{
  --glColor:pink
}
</style>
