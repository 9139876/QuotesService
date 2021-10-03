<template>
    <div class="modal-vue">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Сравнение котировок</h5>
                </div>
                <div class="modal-body">
                    <div>
                        Сравнение
                        <strong>{{ quotesCompareRequest?.tickerAndMarketFirst?.tickerName }}</strong>
                        рынок
                        <strong>{{ quotesCompareRequest?.tickerAndMarketFirst?.marketName }}</strong>
                        и
                        <strong>{{ quotesCompareRequest?.tickerAndMarketSecond?.tickerName }}</strong>
                        рынок
                        <strong>{{ quotesCompareRequest?.tickerAndMarketSecond?.marketName }}</strong>
                        период
                        <strong>{{ quotesCompareRequest?.timeFrameName }}</strong>
                        допустимая погрешность
                        <strong>{{ quotesCompareRequest?.maxDifferencePercent }}%</strong>
                    </div>

                    <div class="margin-text">
                        Начальная дата
                        <strong>{{ quotesCompareResult?.firstComparedDate }}</strong>
                        конечная дата
                        <strong>{{ quotesCompareResult?.lastComparedDate }}</strong>
                    </div>

                    <div v-if="quotesCompareResult?.onlyInFirstTicker?.length > 0">
                        <h5>Присутствуют только в первом инструменте</h5>
                        <div class="small-info-block">
                            <div v-for="(item, index) in quotesCompareResult.onlyInFirstTicker" :key="index">
                                {{ item }}
                            </div>
                        </div>
                    </div>

                    <div v-if="quotesCompareResult?.onlyInFirstTicker?.length > 0">
                        <h5>Присутствуют только во втором инструменте</h5>
                        <div class="small-info-block">
                            <div v-for="(item, index) in quotesCompareResult.onlyInSecondTicker" :key="index">
                                {{ item }}
                            </div>
                        </div>
                    </div>

                    <div v-if="quotesCompareResult?.differences?.length > 0">
                        <h5>Различия</h5>
                        <div class="big-info-block">
                            <div v-for="(item, index) in quotesCompareResult.differences" :key="index">
                                {{ item }}
                            </div>
                        </div>
                    </div>
                    <div v-else>
                        <h5>Различий не обнаружено</h5>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" v-on:click="closeWindow()">Закрыть</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: "ModalCompareQuotesResult",
    props: {
        quotesCompareResultIP: Object,
        quotesCompareRequestIP: Object,
    },
    emits: ["close-command"],
    data: function() {
        return {
            quotesCompareResult: this.quotesCompareResultIP,
            quotesCompareRequest: this.quotesCompareRequestIP,
        };
    },
    methods: {
        closeWindow: function() {
            this.$emit("close-command");
        },
    },
};
</script>

<style scoped>
@import "./../styles/modal.css";

.modal-content {
    margin-top: 1vh;
    min-width: 70vw;
    margin-left: -20vw;
}

.big-info-block {
    max-height: 40vh;
    overflow: auto;
}

.small-info-block {
    max-height: 25vh;
    overflow: auto;
}
</style>
