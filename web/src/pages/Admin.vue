<template>
  <div class="content">
    <div class="container-fluid">
      <card>
        <template slot="header">
          <h4 class="card-title">Admin Dashboard</h4>
          <p class="card-category">
            This dashboard is accessible for the public in interest of
            transparency. Select a region and view localized ground water health ratings. Ratings are provided based on ground water consumption patterns combined with water percolation metrics.
          </p>
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

export default {
  components: {
    AzureMap,
    AzureMapDataSource,
    AzureMapPoint,
    AzureMapHeatMapLayer,
  },
  methods: {},
};
</script>