<template>
    <component v-bind:deckName="helloModel.deckName" v-model="helloModel" :is="helloModel.componentName" transition="fade" transition-mode="out-in"></component>
</template>

<script>
    import game from './Game.vue'
    import home from './Home.vue'
    import decks from './DeckSelection.vue'

    export default {
      name: 'hello',
      components: { game, home, decks },
      data () {
        return {
          helloModel: { componentName: 'home' }
        }
      }
    }
</script>
<style>
    html, body {
        height: 100vh;
        overflow-y:hidden;
    }
</style>