<template>
    <component v-bind:helloModel="helloModel" v-model="helloModel" :is="helloModel.componentName" transition="fade" transition-mode="out-in"></component>
</template>

<script>
    import game from './Game.vue'
    import home from './Home.vue'
    import decks from './DeckSelection.vue'
    import champs from './ChampSelection.vue'
    import gameLobby from './GameLobby.vue'

    export default {
      name: 'hello',
      components: { game, home, decks, champs, gameLobby },
      data () {
        return {
          helloModel: { componentName: 'home', deckName: '', champName: '', username: '', botGame: false }
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

    .fade-enter-active, .fade-leave-active {
      transition: opacity 0.25s ease-out;
    }

    .fade-enter, .fade-leave-to {
      opacity: 0;
    }

    .basic-modal {
        box-shadow: 5px 5px 20px 0px;
        overflow-x: auto;
        position: fixed;
        z-index: 3050;
        overflow: hidden;
        background: white;
        border-radius: 10px;
        padding: 10px;
        display: grid;
        grid-template-rows: 95% 5%;
        transition: opacity .25s ease-in-out;
        -moz-transition: opacity .25s ease-in-out;
        -webkit-transition: opacity .25s ease-in-out;
    }

    .modal-backdrop {
        position: fixed;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        background-color: rgba(0, 0, 0, 0.3);
        display: flex;
        justify-content: center;
        align-items: center;
    }
</style>