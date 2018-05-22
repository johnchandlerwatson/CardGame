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
          helloModel: { componentName: 'home', deckName: '', champName: '', username: '', botGame: false, gameId: null }
        }
      }
    }
</script>
<style>
    html, body {
        height: 100vh;
        overflow-y: hidden;
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

    h1 {
        font-size: 7rem;
    }

    h2 {
        font-size: 5rem;
    }

    h3 {
        font-size: 3rem;
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

    .label-data {
        display: grid;
        grid-template-columns: 50% 50%;
    }

    .card {
        padding: 10px;
        background: linear-gradient(to right, #6b5d5d , #504d4d);
        color: #f3eeee;
        margin-left: 10px;
        margin-top: 10px;
        margin-bottom: 10px;
        border-radius: 10px;
        box-shadow: black 2px 2px 4px 0px;
    }

    .hand-section-horizontal {
        display: flex; 
        flex-direction: row;
        background-color: rgb(156, 156, 162);
        position: relative;
        width: 100%;
    }

    /* Common - Grey Uncommon - Red Rare - Orange Epic - Green Legendary - Blue Mythic - Purple Godly - Gold */
    .common{
        background: linear-gradient(to right, #3e3e41 , #504d4d) !important
    }

    .uncommon{
        background: linear-gradient(to right, #c72424 , #5f0e0e) !important
    }

    .rare{
        background: linear-gradient(to right, #dd9613 , #5c2908) !important
    }

    .epic{
        background: linear-gradient(to right, #266430 , #023006) !important
    }

    .legendary{
        background: linear-gradient(to right, #1c2aec , #06012b) !important
    }

    .mythic{
        background: linear-gradient(to right, #561d7c , #1e0225) !important
    }

    .godly{
        background: linear-gradient(to right, #d89924 , #6b5c1b) !important
    }
</style>