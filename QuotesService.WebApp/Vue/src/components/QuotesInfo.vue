<template>
    <div>
        <template v-if="waitServerResponse">
            <h4>Ожидание ответа от сервера...</h4>
        </template>
        <template v-else>
            <template v-if="quotesInfo?.length > 0">
                <div v-for="(item, index) in quotesInfo" :key="index">
                    <h4>{{ item.timeFrameName }}</h4>
                    <div class="border">
                        <div>
                            Количество котировок:
                            <strong>{{ item.quotesCount }}</strong>
                        </div>

                        <div>
                            Дата первой котировки:
                            <strong>{{ item.firstDate }}</strong>
                        </div>
                        <div>
                            Дата последней котировки:
                            <strong>{{ item.lastDate }}</strong>
                        </div>
                        <div>
                            Минимальная цена:
                            <strong>{{ item.minPrice }}</strong>
                            от
                            <strong>{{ item.minPriceDate }}</strong>
                        </div>
                        <div>
                            Максимальная цена:
                            <strong>{{ item.maxPrice }}</strong>
                            от
                            <strong>{{ item.maxPriceDate }}</strong>
                        </div>
                    </div>
                </div>
            </template>
            <template v-else>
                <h4>Котировок не найдено.</h4>
            </template>
        </template>
    </div>
</template>

<script>
import axios from "axios";

export default {
    name: "QuotesInfo",
    props: {
        currentMarketNameIP: String,
        currentTickerNameIP: String,
    },
    data: function() {
        return {
            getQuotesInfoUrl: "/quotes-getter-api/get-quotes-info",
            quotesInfo: [],
            waitServerResponse: true,
            currentMarketName: this.currentMarketNameIP,
            currentTickerName: this.currentTickerNameIP,
        };
    },
    created: function() {
        this.getQuotesInfo();
    },
    watch: {
        currentMarketNameIP: function() {
            this.currentMarketName = this.currentMarketNameIP;
            this.currentTickerName = this.currentTickerNameIP;
            this.waitServerResponse = true;
            this.getQuotesInfo();
        },
        currentTickerNameIP: function() {
            this.currentTickerName = this.currentTickerNameIP;
            this.waitServerResponse = true;
            this.getQuotesInfo();
        },
    },
    methods: {
        getQuotesInfo: function() {
            if (this.currentMarketName?.length > 0 === false || this.currentTickerName?.length > 0 === false) {
                return;
            }

            let request = {};
            request.marketName = this.currentMarketName;
            request.tickerName = this.currentTickerName;

            this.currentQuotesProvider = {};

            axios
                .post(this.getQuotesInfoUrl, request)
                .then((response) => {
                    if (response?.data) {
                        this.quotesInfo = response.data.filter((x) => x?.quotesCount > 0);
                        this.waitServerResponse = false;
                    } else {
                        alert("Не удалось получить информацию о котировках");
                    }
                })
                .catch((error) => {
                    alert(error);
                });
        },
    },
};
</script>
