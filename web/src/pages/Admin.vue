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
            <base-input-dropdown>
              <template slot="title">
                <i class="fa fa-globe"></i>
                <b class="caret"></b>
                <span class="notification">Select City: {{getfilterCityName}}</span>
              </template>
              <a v-for="city in cities" :key="city.id" v-on:click="selectCity($event, city.id)" class="dropdown-item" href="#">{{city.name}}</a>
            </base-input-dropdown>
            <button v-on:click="loadHeatmap">Load Heatmap</button>
          </p>
        </template>
        <AzureMap :center="getfilterCityLocation" :zoom="12" height="600px">
          <AzureMapDataSource
            :cluster="true"
            :cluster-radius="50"
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
          location: [73.802025, 18.614075],
          id: "pcmc",
        },
        {
          name: "Mumbai",
          location: [72.839863, 18.999543],
          id: "mumbai",
        },
        {
          name: "Thane",
          location: [72.839863, 18.999543],
          id: "thane",
        },
      ],
      selectedCityId: "pune",
      heatmap: [],
    };
  },
  methods: {
    selectCity: function (e, cityId) {
      this.selectedCityId = cityId;
    },
    loadHeatmap: function () {
      console.log("Loading heatmap for city");
      let scope = this;
      axios
        .get("https://bhoojal-data-cdn.azureedge.net/heatmaps/pune/2020Q3.json")
        .then((response) => {
          console.log(JSON.parse(response.data.result));
          let heatmapbuff = JSON.parse(response.data.result);
          scope.heatmap = [];
          heatmapbuff.forEach((item) => {
            scope.heatmap.push(item);
          });
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
    axios
      .get("https://bhoojal-data-cdn.azureedge.net/heatmaps/pune/2020Q3.json")
      .then((response) => {});
  },
};
</script>