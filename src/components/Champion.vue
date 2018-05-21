<template>
    <div class="champion">
        <img :id="champ.Id" class="champ-image" :src="getImage(champ.ChampImage)">
        <p class="champ-health">HLTH: {{champ.Health}}</p>
    </div>
</template>

<script>
  export default {
    name: 'champion',
    props: ['champ'],
    methods: {
      getImage: function (picName) {
        var images = require.context('../assets/', false, /\.png$/)
        return images('./' + picName + '.png')
      }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .champion {
        position: relative;
        width: 20%;
        margin: auto;
        background: linear-gradient(to right,#7d5f9a, #3d2c4e);
        border-radius: 20px;
        color: #d8d8dc;

        border-style: solid;
        border-width: 2px;
        border-top-color: #b398ce;
        border-left-color: #d0bae6;
        border-right-color: #000000;
        border-bottom-color: #5c08c1;
    }

    .champ-image {
        width: 30px;
        margin-top: 5px;
    }

    .champ-health {
        margin-bottom: 2px;
        font-size: .8em;
    }
</style>