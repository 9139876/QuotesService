<template>
    <div v-for="(item, index) in currentTickerInfo?.properties" :key="item.name">
        <label v-bind:for="'tickerInfoPropertyInput' + index" class="form-label">
            <strong>{{ item.name }}</strong>
        </label>
        <input type="text" class="form-control" v-bind:id="'tickerInfoPropertyInput' + index" v-bind:readonly="item.readOnly" v-model="item.value" />
    </div>

    <div class="d-grid gap-2 d-md-flex justify-content-md-end">
        <button type="button" class="btn btn-primary" data-bs-dismiss="modal" v-on:click="setTickerInfo()">
            Сохранить изменения
        </button>
    </div>
</template>

<script>
import axios from "axios";

export default {
    name: "TickerInfo",
    props: {
        currentMarketNameIP: String,
        currentTickerNameIP: String,
    },
    data: function() {
        return {
            currentTickerInfo: {},
            getTickerInfoUrl: "/quotes-getter-api/get-ticker-info",
            setTickerInfoUrl: "/quotes-getter-api/set-ticker-info",
            currentMarketName: this.currentMarketNameIP,
            currentTickerName: this.currentTickerNameIP,
        };
    },
    created: function() {
        this.getTickerInfo();
    },
    watch: {
        currentMarketNameIP: function() {
            this.currentMarketName = this.currentMarketNameIP;
            this.currentTickerName = this.currentTickerNameIP;
            this.getQuotesProviderTasks();
        },
        currentTickerNameIP: function() {
            this.currentTickerName = this.currentTickerNameIP;
            this.getTickerInfo();
        },
    },
    methods: {
        getTickerInfo: function() {
            if (this.currentMarketName?.length > 0 === false || this.currentTickerName?.length > 0 === false) {
                return;
            }

            let request = {};
            request.marketName = this.currentMarketName;
            request.tickerName = this.currentTickerName;

            this.currentTickerInfo = {};

            axios
                .post(this.getTickerInfoUrl, request)
                .then((response) => {
                    if (response.data?.status?.isSuccess === true) {
                        this.currentTickerInfo = response.data;
                    } else {
                        alert("Ошибка: " + response.data?.status?.message ?? "неизвестная ошибка сервера");
                    }
                })
                .catch((error) => {
                    alert(error);
                });
        },
        setTickerInfo: function() {
            if (this.currentTickerInfo.properties?.length > 0 === false) {
                alert("Параметры инструмента не загружены");
                return;
            }

            axios
                .post(this.setTickerInfoUrl, this.currentTickerInfo)
                .then((response) => {
                    if (response.data?.isSuccess === true) {
                        alert("Изменения успешно сохранены");
                    } else {
                        alert("Ошибка: " + response.data?.message ?? "неизвестная ошибка сервера");
                    }
                })
                .catch((error) => {
                    alert(error);
                });
        },
    },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
button{
    margin: 0.8vmin 0px 0.8vmin 0px;
}
</style>
