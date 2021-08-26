<template>
    <div class="modal-vue">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Удаление инструмента</h5>
                </div>
                <div class="modal-body">
                    <h6>Вы уверены, что хотите удалить инструмент {{ currentTickerName }}?</h6>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" v-on:click="deleteTicker()">Удалить</button>
                    <button type="button" class="btn btn-secondary" v-on:click="closeWindow()">Отмена</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import axios from "axios";

export default {
    name: "ModalDeleteTicker",
    props: {
        currentMarketNameIP: String,
        currentTickerNameIP: String,
    },
    emits: ["close-command", "submit-close-command"],
    data: function() {
        return {
            currentMarketName: this.currentMarketNameIP,
            currentTickerName: this.currentTickerNameIP,
            deleteTickerUrl: "/quotes-getter-api/delete-ticker",
        };
    },
    methods: {
        deleteTicker: function() {
            if (this.currentMarketName?.length > 0 === false) {
                alert("Рынок не выбран!");
                return;
            }

            if (this.currentTickerName?.length > 0 === false) {
                return;
            }

            let request = {};
            request.marketName = this.currentMarketName;
            request.tickerName = this.currentTickerName;

            axios
                .post(this.deleteTickerUrl, request)
                .then((response) => {
                    if (response.data?.isSuccess === true) {
                        alert("Инструмент " + this.currentTickerName + " успешно удален!");
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
