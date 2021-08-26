<template>
    <!-- <img alt="Vue logo" src="./assets/logo.png" /> -->
    <div class="container">
        <Markets />
    </div>
</template>

<script>
import Markets from "./components/Markets.vue";

export default {
    name: "App",
    components: {
        Markets,        
    },    
};
</script>

<style>
@import "./styles/bootstrap.min.css";

body {
    margin-top: 60px;
}
input {
    margin: 0.5vmin 0px;
}
.border {
    padding: 0.8vmin;
    margin: 0.8vmin 0px;
}
.col-md-7 {
    margin: 1.5vmin 0 3vmin 0;
}
</style>
