<template>
    <div class="modal-vue">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Удаление рынка</h5>
                </div>
                <div class="modal-body">
                    <h6>Вы уверены, что хотите удалить рынок {{ currentMarketName }}?</h6>
                    <div v-if="currentMarketTickers.length > 0">Будут также удалены следующие инструменты:</div>
                    <div v-for="(item, index) in currentMarketTickers" :key="index">- {{ item }}</div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" v-on:click="deleteMarket()">Удалить</button>
                    <button type="button" class="btn btn-secondary" v-on:click="closeWindow()">Отмена</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import axios from "axios";

export default {
    name: "ModalDeleteMarket",
    props: {
        currentMarketNameIP: String,
        currentMarketTickersIP: [],
    },
    emits: ["close-command", "submit-close-command"],
    data: function() {
        return {
            deleteMarketUrl: "/quotes-getter-api/delete-market",
            currentMarketName: this.currentMarketNameIP,
            currentMarketTickers: this.currentMarketTickersIP,
        };
    },
    methods: {
        deleteMarket: function() {
            if (this.currentMarketName?.length > 0 === false) {
                return;
            }

            axios
                .get(this.deleteMarketUrl + "?marketName=" + this.currentMarketName)
                .then((response) => {
                    if (response.data?.isSuccess === true) {
                        alert("Рынок " + this.currentMarketName + " успешно удален!");                        
                        this.submitcloseWindow();
                    } else {
                        alert("Ошибка: " + response.data?.message ?? "неизвестная ошибка сервера");
                    }
                })
                .catch((error) => {
                    alert(error);
                });
        },
        closeWindow: function() {
            this.$emit("close-command");
        },
        submitcloseWindow: function() {
            this.$emit("submit-close-command");
        },
    },
};
</script>

<style scoped>
@import "./../styles/modal.css";
</style>
