<template>
    <div class="played-card">
        <span class="centered">{{card.Name}}</span>
        <div class="stats">
            <p>HLTH: {{card.Health}}</p>
            <p style="text-align: right;">DMG: {{card.Damage}}</p>
        </div>
    </div>
</template>

<script>
  export default {
    name: 'playedCard',
    props: ['card', 'isEnemy']
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .played-card {
        background: linear-gradient(to right, #6b5d5d , #504d4d);
        color: #f3eeee;
        margin: 10px;
        border-radius: 4px;
        padding: 10px;
        width: 125px;
        font-size: .8em;
    }

    .stats {
        display: grid;
        grid-template-columns: 50% 50%;
    }

    .stats > p {
        margin: 0;
    }
</style>