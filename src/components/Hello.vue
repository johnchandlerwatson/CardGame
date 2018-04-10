<template>
    <component v-model="currentView" :is="currentView" transition="fade" transition-mode="out-in"></component>
</template>

<script>
    import game from './Game.vue'
    import home from './Home.vue'

    export default {
      name: 'hello',
      components: { game, home },
      data () {
        return {
          currentView: 'home'
        }
      }
    }
</script>
<style>
    html, body {
        height: 100vh;
    }
</style>