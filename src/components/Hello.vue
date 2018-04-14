<template>
    <component v-bind:helloModel="helloModel" v-model="helloModel" :is="helloModel.componentName" transition="fade" transition-mode="out-in"></component>
</template>

<script>
    import game from './Game.vue'
    import home from './Home.vue'
    import decks from './DeckSelection.vue'
    import champs from './ChampSelection.vue'

    export default {
      name: 'hello',
      components: { game, home, decks, champs },
      data () {
        return {
          helloModel: { componentName: 'home', deckName: '', champName: '' }
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