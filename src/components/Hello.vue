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
        font-family: 'East Sea Dokdo', cursive;
    }

    .centered {
        display: grid;
        align-content: center;
        align-items: center;
        text-align: center;
    }

    h1, h2, h3, h4 {
        font-family: 'Bungee', cursive;
    }

    p, span, button, label, div {
        font-family: 'Jua', sans-serif;
    }
</style>