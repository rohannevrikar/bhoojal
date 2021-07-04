<template>
  <div class="content">
    <div class="container-fluid">
      <card>
        <template slot="header">
          <h4 class="card-title">Manage Outlets</h4>
          <p class="card-category">
            View and manage outlets registered on the system.
          </p>
        </template>
        <div class="row">
          <div class="col-8">
            <AzureMap :center="getfilterCityLocation" :zoom="12" height="600px">
              <AzureMapDataSource
                :cluster="true"
                :cluster-radius="50"
                :cluster-max-zoom="5"
              >
                <AzureMapHeatMapLayer></AzureMapHeatMapLayer>
              </AzureMapDataSource>
            </AzureMap>
          </div>
          <div class="col-4">
            <stats-card>
              <div slot="header" class="icon-info">
                <i class="nc-icon nc-favourite-28 text-primary"></i>
              </div>
              <div slot="content">
                <p class="card-category">Followers</p>
                <h4 class="card-title">+45</h4>
              </div>
              <div slot="footer"><i class="fa fa-refresh"></i>Updated now</div>
            </stats-card>
          </div>
        </div>
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