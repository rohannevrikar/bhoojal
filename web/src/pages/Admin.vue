<template>
  <div class="content">
    <div class="container-fluid">
      <card>
        <template slot="header">
          <h4 class="card-title">GPR Data Dashboard</h4>
          <p class="card-category">
            Select a region and view localized ground water health
            ratings. Ratings are provided based on ground water consumption
            patterns combined with water percolation metrics.
          </p>
          <div>
            <div v-if="errorMessage" class="alert alert-warning">
              <span><b> Warning - </b> {{errorMessage}}"</span>
            </div>
            <base-input-dropdown>
              <template slot="title">
                <i class="fa fa-globe"></i>
                <b class="caret"></b>
                <span class="notification">Select City: {{getfilterCityName}}</span>
              </template>
              <a v-for="city in cities" :key="city.id" v-on:click="selectCity($event, city.id)" class="dropdown-item" href="#">{{city.name}}</a>
            </base-input-dropdown>
            <base-input-dropdown>
              <template slot="title">
                <b class="caret"></b>
                <span class="notification">Select Quarter: {{selectedQuarter}}</span>
              </template>
              <a v-for="q in quarters" :key="q" v-on:click="selectQuarter($event, q)" class="dropdown-item" href="#">{{q}}</a>
            </base-input-dropdown>
          </div>
        </template>
        <AzureMap :center="getfilterCityLocation" :zoom="12" height="600px">
          <AzureMapDataSource
            :cluster="true"
            :cluster-radius="1"
            :cluster-max-zoom="5"
          >
            <AzureMapPoint
              v-for="(item, i) in heatmap"
              :key="i + 10"
              :longitude="item.lng"
              :latitude="item.lat"
            />
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
          location: [73.802025, 18.614075],
          id: "pune",
        },
        {
          name: "PCMC",
          location: [73.78663, 18.608627],
          id: "pcmc",
        },
        {
          name: "Mumbai",
          location: [72.839863, 18.999543],
          id: "mumbai",
        },
        {
          name: "Thane",
          location: [72.95626, 19.212616],
          id: "thane",
        },
      ],
      quarters: ["2020Q2","2020Q3","2020Q4","2021Q1","2021Q2"],
      selectedCityId: "pune",
      selectedQuarter: "2021Q2",
      heatmap: [],
      errorMessage: null
    };
  },
  methods: {
    reset: function () {
      this.heatmap = [];
      this.loadHeatmap();
    },
    selectCity: function (e, cityId) {
      this.selectedCityId = cityId;
      this.reset();
    },
    selectQuarter: function (e, quarter) {
      this.selectedQuarter = quarter;
      this.reset();
    },
    loadHeatmap: function () {
      console.log("Loading heatmap for city");
      let scope = this;
      axios
        .get("https://bhoojal-data-cdn.azureedge.net/heatmaps/"+scope.selectedCityId+"/"+scope.selectedQuarter+".json")
        .then((response) => {
          console.log(JSON.parse(response.data.result));
          let heatmapbuff = JSON.parse(response.data.result);
          scope.heatmap = [];
          heatmapbuff.forEach((item) => {
            scope.heatmap.push({ lat: parseFloat(item.lat), lng: parseFloat(item.lng) });
          });
        })
        .catch((error) => {
          scope.errorMessage = "GPR data for city " + scope.selectedCityId + " for the quarter " + scope.selectedQuarter + " is not available. Pune and PCMC have been onboarded with GPR data.";
          console.log("Error", error)
        });
    },
  },
  computed: {
    getfilterCityLocation() {
      let location = null;
      let context = this;
      context.cities.forEach((city) => {
        if (city.id == context.selectedCityId) {
          location = city.location;
        }
      });
      return location;
    },
    getfilterCityName() {
      let name = null;
      let context = this;
      context.cities.forEach((city) => {
        if (city.id == context.selectedCityId) {
          name = city.name;
        }
      });
      return name;
    },
  },
  mounted() {
    console.log(process.env.OUTLET_API_CODE);
    this.loadHeatmap();
  },
};
</script>