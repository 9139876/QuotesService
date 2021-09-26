<template>
    <div>
        <h4>Рынок</h4>
        <form class="row g-3">
            <div class="col-md-7">
                <select class="form-select form-select-md" v-model="currentMarketName">
                    <option disabled value="">Выбор рынка</option>
                    <option v-for="item in markets.map((x) => x.marketName)" :key="item">
                        {{ item }}
                    </option>
                </select>
            </div>

            <div class="col-md-1">
                <button type="button" class="btn btn-primary" v-on:click="modalAddMarketVisible = true">
                    Добавить
                </button>
            </div>

            <!-- Modal Add Market-->
            <ModalAddMarket v-if="modalAddMarketVisible" @close-command="modalAddMarketVisible = false" @submit-close-command="onMarketAdded" />

            <div class="col-md-1" v-if="currentMarketName?.length > 0">
                <button type="button" class="btn btn-danger" v-on:click="modalDeleteMarketVisible = true">
                    Удалить
                </button>
            </div>

            <!-- Modal Delete Market-->
            <ModalDeleteMarket
                v-if="modalDeleteMarketVisible"
                :currentMarketNameIP="currentMarketName"
                :currentMarketTickersIP="currentMarketTickers"
                @close-command="modalDeleteMarketVisible = false"
                @submit-close-command="onMarketDeleted"
            />
        </form>

        <template v-if="currentMarketName?.length > 0">
            <h4>Инструмент</h4>
            <form class="row g-3">
                <div class="col-md-7">
                    <select class="form-select form-select-md" v-model="currentTickerName">
                        <option disabled value="">Выбор инструмента</option>
                        <option v-for="item in currentMarketTickers" :key="item">
                            {{ item }}
                        </option>
                    </select>
                </div>

                <div class="col-md-1">
                    <button type="button" class="btn btn-primary" v-on:click="modalAddTickerVisible = true">
                        Добавить
                    </button>
                </div>

                <!-- Modal Add Ticker-->
                <ModalAddTicker
                    v-if="modalAddTickerVisible"
                    :currentMarketNameIP="currentMarketName"
                    @close-command="modalAddTickerVisible = false"
                    @submit-close-command="onTickerAdded"
                />

                <div class="col-md-1" v-if="currentTickerName?.length > 0">
                    <button type="button" class="btn btn-danger" v-on:click="modalDeleteTickerVisible = true">
                        Удалить
                    </button>
                </div>

                <!-- Modal Delete Ticker-->
                <ModalDeleteTicker
                    v-if="modalDeleteTickerVisible"
                    :currentMarketNameIP="currentMarketName"
                    :currentTickerNameIP="currentTickerName"
                    @close-command="modalDeleteTickerVisible = false"
                    @submit-close-command="onTickerDeleted"
                />
            </form>
        </template>

        <template v-if="currentTickerName?.length > 0">
            <ul class="nav nav-tabs" role="tablist">
                <li class="nav-item">
                    <button class="nav-link" data-bs-toggle="tab" type="button" v-on:click="currentTab = 'tickerInfo'">
                        Информация об инструменте
                    </button>
                </li>
                <li class="nav-item">
                    <button class="nav-link" data-bs-toggle="tab" type="button" v-on:click="currentTab = 'quotesProvider'">
                        Источник котировок
                    </button>
                </li>
                <li class="nav-item">
                    <button class="nav-link" data-bs-toggle="tab" type="button" v-on:click="currentTab = 'quotesProviderTasks'">
                        Задачи получения котировок
                    </button>
                </li>                
                <li class="nav-item">
                    <button class="nav-link" data-bs-toggle="tab" type="button" v-on:click="currentTab = 'quotesInfo'">
                        Информация о котировках
                    </button>
                </li>
                <li class="nav-item">
                    <button class="nav-link" data-bs-toggle="tab" type="button" v-on:click="currentTab = 'quotesComparator'">
                        Сравнение котировок
                    </button>
                </li>
                <li class="nav-item">
                    <button class="nav-link" data-bs-toggle="tab" type="button" v-on:click="currentTab = 'loadFromFile'">
                        Загрузка котировок из файла
                    </button>
                </li>
            </ul>
            <div class="tab-content" id="myTabContent">
                <!-- TickerInfo -->
                <div class="container border" v-if="currentTab === 'tickerInfo'">
                    <TickerInfo :currentMarketNameIP="currentMarketName" :currentTickerNameIP="currentTickerName"></TickerInfo>
                </div>

                <!-- Quotes provider -->
                <div v-if="currentTab === 'quotesProvider'">
                    <QuotesProvider
                        :currentMarketNameIP="currentMarketName"
                        :currentTickerNameIP="currentTickerName"
                        @ticker-renamed="onReanamedTicker"
                    ></QuotesProvider>
                </div>

                <!-- Quotes provider tasks -->
                <div v-if="currentTab === 'quotesProviderTasks'">
                    <QuotesProviderTasks :currentMarketNameIP="currentMarketName" :currentTickerNameIP="currentTickerName"></QuotesProviderTasks>
                </div>

                <!-- Информация о котировках -->
                <div v-if="currentTab === 'quotesInfo'">
                    <QuotesInfo :currentMarketNameIP="currentMarketName" :currentTickerNameIP="currentTickerName"></QuotesInfo>
                </div>

                <!-- Сравнение котировок -->
                <div v-if="currentTab === 'quotesComparator'">
                    Сравнение котировок
                </div>

                <!-- Загрузка котировок из файла -->
                <div v-if="currentTab === 'loadFromFile'">
                    Загрузка котировок из файла
                </div>
            </div>
        </template>
    </div>
</template>

<script>
import axios from "axios";

import TickerInfo from "./TickerInfo.vue";
import QuotesProvider from "./QuotesProvider.vue";
import QuotesProviderTasks from "./QuotesProviderTasks.vue";
import QuotesInfo from "./QuotesInfo.vue";
import ModalAddMarket from "./ModalAddMarket.vue";
import ModalDeleteMarket from "./ModalDeleteMarket.vue";
import ModalAddTicker from "./ModalAddTicker.vue";
import ModalDeleteTicker from "./ModalDeleteTicker.vue";

export default {
    name: "Markets",
    components: {
        TickerInfo,
        QuotesProvider,
        QuotesProviderTasks,
        QuotesInfo,
        ModalAddMarket,
        ModalDeleteMarket,
        ModalAddTicker,
        ModalDeleteTicker,
    },
    data: function() {
        return {
            getMarketsUrl: "/quotes-getter-api/get-markets",
            markets: [],
            currentMarketName: "",
            currentTickerName: "",
            currentTab: "",
            modalAddMarketVisible: false,
            modalDeleteMarketVisible: false,
            modalAddTickerVisible: false,
            modalDeleteTickerVisible: false,
        };
    },
    created: function() {
        this.getMarkets();
    },
    watch: {
        currentMarketName: function() {
            this.currentTickerName = "";
        },
    },
    computed: {
        currentMarketTickers: function() {
            const vue = this;

            if (vue.currentMarketName?.length > 0) {
                return vue.markets.filter((x) => x.marketName === vue.currentMarketName)[0].tickersNames;
            } else {
                return [];
            }
        },
    },
    methods: {
        getMarkets: function() {
            axios
                .get(this.getMarketsUrl)
                .then((response) => {
                    this.markets = response.data;
                })
                .catch((error) => {
                    alert(error);
                });
        },
        onMarketAdded: function(addedMarketName) {
            this.getMarkets();
            this.currentMarketName = addedMarketName;
            this.modalAddMarketVisible = false;
        },
        onMarketDeleted: function() {
            this.getMarkets();
            this.currentMarketName = "";
            this.modalDeleteMarketVisible = false;
        },
        onTickerAdded: function(addedTickerName) {
            this.getMarkets();
            this.currentTickerName = addedTickerName;
            this.modalAddTickerVisible = false;
        },
        onTickerDeleted: function() {
            this.getMarkets();
            this.currentTickerName = "";
            this.modalDeleteTickerVisible = false;
        },
        onReanamedTicker: function(newName) {
            this.getMarkets();
            this.currentTickerName = newName;
        },
    },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
