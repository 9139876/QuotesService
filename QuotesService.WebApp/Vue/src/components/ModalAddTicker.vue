<template>
    <div class="modal-vue">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Добавление инструмента</h5>
                </div>
                <div class="modal-body">
                    <label for="addedTickerNameInput" class="form-label">Название инструмента</label>
                    <input type="text" class="form-control" id="addedTickerNameInput" v-model="addedTickerName" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" v-on:click="addTicker()">Добавить</button>
                    <button type="button" class="btn btn-secondary" v-on:click="closeWindow()">Отмена</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import axios from "axios";

export default {
    name: "ModalAddTicker",
    props: {
        currentMarketNameIP: String,
    },
    emits: ["close-command", "submit-close-command"],
    data: function() {
        return {
            addTickerUrl: "/quotes-getter-api/add-ticker",
            currentMarketName: this.currentMarketNameIP,
            addedTickerName: "",
        };
    },
    methods: {
        addTicker: function() {
            if (this.currentMarketName?.length > 0 === false) {
                alert("Рынок не выбран!");
                return;
            }

            if (this.addedTickerName?.length > 0 === false) {
                alert("Имя добавляемого инструмента не должно быть пустым!");
                return;
            }

            let request = {};
            request.marketName = this.currentMarketName;
            request.tickerName = this.addedTickerName;

            axios
                .post(this.addTickerUrl, request)
                .then((response) => {
                    if (response.data?.isSuccess === true) {
                        alert("Инструмент " + this.addedTickerName + " успешно добавлен!");
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
            this.$emit("submit-close-command", this.addedTickerName);
        },
    },
};
</script>

<style scoped>
@import "./../styles/modal.css";
</style>
