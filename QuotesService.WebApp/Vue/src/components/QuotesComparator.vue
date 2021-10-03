<template>
    <div>
        <h4>Период</h4>
        <form class="row g-3">
            <div class="col-md-3">
                <select class="form-select form-select-md" v-model="currentTimeFrameName">
                    <option disabled value="">Выбор периода</option>
                    <option v-for="(item, index) in timeFrames" :key="index">
                        {{ item.name }}
                    </option>
                </select>
            </div>
        </form>

        <div class="margin-text">
            Инструмент
            <strong>{{ firstTickerName }}</strong>
            рынок
            <strong>{{ firstMarketName }}</strong>

            <template v-if="quotesInfoFirst">
                содержит
                <strong>{{ quotesInfoFirst.quotesCount }}</strong>
                котировок
                <template v-if="quotesInfoFirst.quotesCount">
                    с
                    <strong>{{ quotesInfoFirst.firstDate }}</strong>
                    по
                    <strong>{{ quotesInfoFirst.lastDate }}</strong>
                </template>
            </template>
        </div>

        <template v-if="currentTimeFrameType?.length > 0">
            <h5><strong>СРАВНИВАЕТСЯ С</strong></h5>

            <h5>Рынок</h5>
            <form class="row g-3">
                <div class="col-md-6">
                    <select class="form-select form-select-md" v-model="secondMarketName">
                        <option disabled value="">Выбор рынка</option>
                        <option v-for="(item, index) in markets.map((x) => x.marketName)" :key="index">
                            {{ item }}
                        </option>
                    </select>
                </div>
            </form>

            <template v-if="secondMarketName?.length > 0">
                <h5>Инструмент</h5>
                <form class="row g-3">
                    <div class="col-md-6">
                        <select class="form-select form-select-md" v-model="secondTickerName">
                            <option disabled value="">Выбор инструмента</option>
                            <option v-for="(item, index) in secondMarketTickers" :key="index">
                                {{ item }}
                            </option>
                        </select>
                    </div>
                </form>
            </template>

            <div class="margin-text" v-if="secondTickerName?.length > 0">
                Инструмент
                <strong>{{ secondTickerName }}</strong>
                рынок
                <strong>{{ secondMarketName }}</strong>

                <template v-if="quotesInfoSecond">
                    содержит
                    <strong>{{ quotesInfoSecond.quotesCount }}</strong>
                    котировок
                    <template v-if="quotesInfoSecond.quotesCount">
                        с
                        <strong>{{ quotesInfoSecond.firstDate }}</strong>
                        по
                        <strong>{{ quotesInfoSecond.lastDate }}</strong>
                    </template>
                </template>
            </div>

            <button
                v-if="quotesInfoFirst?.quotesCount && quotesInfoSecond?.quotesCount && !maxDifferencePercentInputNotCorrect"
                type="button"
                class="btn btn-primary"
                v-on:click="compareQuotes"
            >
                Сравнить
            </button>
        </template>

        <form class="row g-3">
            <div class="col-md-3">
                <label for="maxDifferencePercentInput" class="form-label">
                    <strong>Допустимая погрешность, %</strong>
                </label>
                <input type="text" class="form-control" id="maxDifferencePercentInput" v-model="maxDifferencePercent" />
                <div class="error-text" v-if="maxDifferencePercentInputNotCorrect">Значение дожно быть целым числом от 0 до 100.</div>
            </div>
        </form>

        <ModalCompareQuotesResult
            v-if="modalCompareQuotesResultVisible"
            @close-command="modalCompareQuotesResultVisible = false"
            :quotesCompareResultIP="quotesCompareResult"
            :quotesCompareRequestIP="quotesCompareRequest"
        />
    </div>
</template>

<script>
import axios from "axios";

import ModalCompareQuotesResult from "./ModalCompareQuotesResult.vue";

export default {
    name: "QuotesComparator",
    components: {
        ModalCompareQuotesResult,
    },
    props: {
        currentMarketNameIP: String,
        currentTickerNameIP: String,
        marketsIP: Array,
    },
    data: function() {
        return {
            firstMarketName: this.currentMarketNameIP,
            firstTickerName: this.currentTickerNameIP,
            secondMarketName: "",
            secondTickerName: "",
            markets: this.marketsIP,
            getTimeFramesUrl: "/quotes-getter-api/get-timeframes",
            getQuotesInfoByTfUrl: "/quotes-getter-api/get-quotes-info-by-tf",
            compareQuotesUrl: "/quotes-getter-api/compare-quotes",
            timeFrames: [],
            currentTimeFrameName: "",
            currentTimeFrameType: "",
            quotesInfoFirst: null,
            quotesInfoSecond: null,
            quotesCompareRequest: null,
            quotesCompareResult: "",
            modalCompareQuotesResultVisible: false,
            maxDifferencePercent: 0,
            // maxDifferencePercentInputNotCorrect: false,
        };
    },
    computed: {
        secondMarketTickers: function() {
            if (this.secondMarketName?.length > 0) {
                return this.markets.filter((x) => x.marketName === this.secondMarketName)[0].tickersNames;
            } else {
                return [];
            }
        },
        maxDifferencePercentInputNotCorrect: function() {
            return (
                this.maxDifferencePercent.length == 0 ||
                isNaN(this.maxDifferencePercent) ||
                this.maxDifferencePercent < 0 ||
                this.maxDifferencePercent > 100
            );
        },
    },
    created: function() {
        this.getTimeFrames();
    },
    watch: {
        currentTimeFrameName: function() {
            this.currentTimeFrameType = this.timeFrames.filter((x) => x.name == this.currentTimeFrameName).map((x) => x.type)[0];
            this.getQuotesInfoFirst();
            this.getQuotesInfoLast();
        },
        secondMarketName: function() {
            this.secondTickerName = "";
            this.quotesInfoSecond = null;
        },
        secondTickerName: function() {
            this.getQuotesInfoLast();
        },
        firstTickerName: function() {
            this.secondMarketName = "";
            this.getQuotesInfoFirst();
            this.getQuotesInfoLast();
        },
        currentTickerNameIP: function() {
            this.firstMarketName = this.currentMarketNameIP;
            this.firstTickerName = this.currentTickerNameIP;
        },
    },
    methods: {
        getTimeFrames: function() {
            axios
                .get(this.getTimeFramesUrl)
                .then((response) => {
                    this.timeFrames = response.data;
                })
                .catch((error) => {
                    alert(error);
                });
        },
        getQuotesInfoFirst: function() {
            if (this.currentTimeFrameType?.length > 0) {
                let request = {};
                request.marketName = this.firstMarketName;
                request.tickerName = this.firstTickerName;
                request.timeFrame = this.currentTimeFrameType;

                axios
                    .post(this.getQuotesInfoByTfUrl, request)
                    .then((response) => {
                        this.quotesInfoFirst = response.data;
                    })
                    .catch((error) => {
                        alert(error);
                    });
            }
        },
        getQuotesInfoLast: function() {
            if (this.currentTimeFrameType?.length > 0 && this.secondTickerName?.length > 0) {
                let request = {};
                request.marketName = this.secondMarketName;
                request.tickerName = this.secondTickerName;
                request.timeFrame = this.currentTimeFrameType;

                axios
                    .post(this.getQuotesInfoByTfUrl, request)
                    .then((response) => {
                        this.quotesInfoSecond = response.data;
                    })
                    .catch((error) => {
                        alert(error);
                    });
            }
        },
        compareQuotes: function() {
            if (this.maxDifferencePercentInputNotCorrect) {
                return;
            }

            this.quotesCompareRequest = {};

            this.quotesCompareRequest.tickerAndMarketFirst = {};
            this.quotesCompareRequest.tickerAndMarketFirst.marketName = this.firstMarketName;
            this.quotesCompareRequest.tickerAndMarketFirst.tickerName = this.firstTickerName;

            this.quotesCompareRequest.tickerAndMarketSecond = {};
            this.quotesCompareRequest.tickerAndMarketSecond.marketName = this.secondMarketName;
            this.quotesCompareRequest.tickerAndMarketSecond.tickerName = this.secondTickerName;

            this.quotesCompareRequest.timeFrame = this.currentTimeFrameType;
            this.quotesCompareRequest.timeFrameName = this.currentTimeFrameName;

            this.quotesCompareRequest.maxDifferencePercent = this.maxDifferencePercent;

            axios
                .post(this.compareQuotesUrl, this.quotesCompareRequest)
                .then((response) => {
                    if (response.data?.isSuccess === true) {
                        this.quotesCompareResult = response.data;
                        this.modalCompareQuotesResultVisible = true;
                    } else {
                        alert(response.data?.message ?? "Неизвестная ошибка сервера");
                    }
                })
                .catch((error) => {
                    alert(error);
                });
        },
    },
};
</script>

<style>
.margin-text {
    margin-top: 3vmin;
    margin-bottom: 3vmin;
}
.error-text {
    color: red;
}
</style>
