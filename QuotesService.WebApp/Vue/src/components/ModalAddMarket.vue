<template>
    <div class="modal-vue">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Добавление рынка</h5>
                </div>
                <div class="modal-body">
                    <label for="addedMarketNameInput" class="form-label">Название рынка</label>
                    <input type="text" class="form-control" id="addedMarketNameInput" v-model="addedMarketName" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" v-on:click="addMarket()">Добавить</button>
                    <button type="button" class="btn btn-secondary" v-on:click="closeWindow()">Отмена</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import axios from "axios";

export default {
    name: "ModalAddMarket",
    data: function() {
        return {
            addMarketUrl: "/quotes-getter-api/add-market",
            addedMarketName: "",
        };
    },
    emits: ["close-command", "submit-close-command"], 
    methods: {
        addMarket: function() {
            if (this.addedMarketName?.length > 0 === false) {
                alert("Имя добавляемого рынка не должно быть пустым!");
                return;
            }

            axios
                .get(this.addMarketUrl + "?marketName=" + this.addedMarketName)
                .then((response) => {
                    if (response.data?.isSuccess === true) {
                        alert("Рынок " + this.addedMarketName + " успешно добавлен!");                        
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
            this.$emit('close-command');
        },
        submitcloseWindow:function(){
            this.$emit('submit-close-command', this.addedMarketName);
        }
    },
};
</script>

<style scoped>
@import "./../styles/modal.css";
</style>
