<template>
  <div class="content">
    <div class="container-fluid">
      <card>
        <template slot="header">
          <h4 class="card-title">Admin Dashboard</h4>
          <p class="card-category">
            This dashboard is accessible for the public in interest of
            transparency. Select a region and view localized ground water health
            ratings. Ratings are provided based on ground water consumption
            patterns combined with water percolation metrics.
          </p>
          <p>
            <button v-on:click="loadHeatmap">Load Heatmap</button>
          </p>
          <div v-for="(p, i) in heatmap" :key="i + 10">
            <span>{{p}}</span>
          </div>
        </template>
        <AzureMap :center="getfilterCityLocation" :zoom="12" height="600px">
          <AzureMapDataSource
            :cluster="true"
            :cluster-radius="50"
            :cluster-max-zoom="5"
          >
            <AzureMapHeatMapLayer></AzureMapHeatMapLayer>
          </AzureMapDataSource>
        </AzureMap>
      </card>
    </div>
  </div>
</template>

<script>
import {
  AzureMap,
  AzureMapDataSource,
  AzureMapPoint,
  AzureMapHeatMapLayer,
} from "vue-azure-maps";

import axios from "axios";

export default {
  components: {
    AzureMap,
    AzureMapDataSource,
    AzureMapPoint,
    AzureMapHeatMapLayer,
  },
  data() {
    return {
      cities: [
        {
          name: "Pune",
          id: "pune",
        },
        {
          name: "PCMC",
          id: "pcmc",
        },
        {
          name: "Mumbai",
          id: "mumbai",
        },
        {
          name: "Thane",
          id: "thane",
        },
      ],
      selectedCityId: "pune",
      heatmap: []
    };
  },
  methods: {
    loadHeatmap: function () {
      console.log("Loading heatmap for city");
      let scope = this;
      axios
        .get("https://bhoojal-data-cdn.azureedge.net/heatmaps/pune/2020Q3.json")
        .then((response) => {
          console.log(JSON.parse(response.data.result));
          scope.heatmap = JSON.parse(response.data.result);
        });
    },
  },
  mounted() {
    axios
      .get("https://bhoojal-data-cdn.azureedge.net/heatmaps/pune/2020Q3.json")
      .then((response) => {});
  },
};
</script>