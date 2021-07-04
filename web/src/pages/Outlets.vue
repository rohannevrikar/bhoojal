<template>
  <div class="content">
    <div class="container-fluid">
      <card>
        <template slot="header">
          <h4 class="card-title">Manage Outlets</h4>
          <p class="card-category">
            View outlets registered on the system.
          </p>
          <div>
            <div v-if="errorMessage" class="alert alert-warning">
              <span><b> Warning - </b> {{ errorMessage }}"</span>
            </div>
            <base-input-dropdown>
              <template slot="title">
                <i class="fa fa-globe"></i>
                <b class="caret"></b>
                <span class="notification"
                  >Select City: {{ getfilterCityName }}</span
                >
              </template>
              <a
                v-for="city in cities"
                :key="city.id"
                v-on:click="selectCity($event, city.id)"
                class="dropdown-item"
                href="#"
                >{{ city.name }}</a
              >
            </base-input-dropdown>
          </div>
        </template>
        <div class="row">
          <div class="col-8">
            <AzureMap :center="getfilterCityLocation" :zoom="12" height="600px">
              <AzureMapDataSource>
                <AzureMapSymbolLayer
                  :options="symbolLayerOptions"
                  @mousedown="clickMarker"
                />
                <template v-for="point in outlets">
                  <!-- Add Points to the Data Source -->
                  <AzureMapPoint
                    :key="point.id"
                    :longitude="point.location.coordinates[0]"
                    :latitude="point.location.coordinates[1]"
                    :properties="point.properties"
                  />
                  <!-- Add a Popup to the map for every Point -->
                  <AzureMapPopup
                    :key="`Popup-${point.id}`"
                    v-model="point.properties.isPopupOpen"
                    :position="[
                      point.location.coordinates[0],
                      point.location.coordinates[1],
                    ]"
                    :pixel-offset="[0, -18]"
                    :close-button="true"
                    class="AzureMapPopup"
                  >
                    <p>
                      <strong>Outlet ID #{{ point.id }}</strong>
                    </p>
                    <p>
                      {{ point.properties.description }}
                    </p>
                  </AzureMapPopup>
                </template>
              </AzureMapDataSource>
            </AzureMap>
          </div>
          <div class="col-4">
            <card v-if="previewOutlet.id" class="card">
              <div class="author">
                <a href="#">
                  <h4 class="title">
                    Outlet #{{previewOutlet.id}}<br />
                    <small>{{previewOutlet.city}}</small>
                  </h4>
                </a>
              </div>
              <p class="description">
                <strong>Quality Score:</strong>{{previewOutlet.qualityScore}}<br />
                <strong>Quantity Score:</strong>{{previewOutlet.quantityScore}}<br />
                <strong>pH Level:</strong>{{previewOutlet.phLevel}}<br />
                <strong>Hardness:</strong>{{previewOutlet.hardness}}<br />
                <strong>Quota:</strong>{{previewOutlet.availableQuota}}<br />
                <strong>Consumed Quota:</strong>{{previewOutlet.consumedQuota}}<br />
              </p>
              <div
                slot="footer"
                class="text-center d-flex justify-content-center"
              >
              </div>
            </card>
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
  AzureMapPopup,
  AzureMapHtmlMarker,
  AzureMapSymbolLayer,
} from "vue-azure-maps";

import Card from "src/components/Cards/Card.vue";
import StatsCard from "src/components/Cards/StatsCard.vue";

import axios from "axios";

export default {
  components: {
    AzureMap,
    AzureMapDataSource,
    AzureMapPoint,
    AzureMapHeatMapLayer,
    AzureMapSymbolLayer,
    AzureMapHtmlMarker,
    AzureMapPopup,
    Card,
    StatsCard,
  },
  data() {
    return {
      symbolLayerOptions: {
        iconOptions: {
          ignorePlacement: true,
          allowOverlap: true,
          image: "pin-red",
        },
      },
      cities: [
        {
          name: "Pune",
          location: [73.873314, 18.50765],
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
      selectedCityId: "pune",
      errorMessage: null,
      outlets: [],
      previewOutlet: {
        id: null,
        city: null,
        qualityScore: null,
        quantityScore: null,
        phLevel: null,
        hardness: null,
        availableQuota: null,
        consumedQuota: null,
      },
    };
  },
  methods: {
    clickMarker: function (e, a) {
      console.log(e, e.shapes.length);
      if (e.shapes && e.shapes.length > 0) {
        console.log(e.shapes[0].getProperties());
        this.loadOutlet(e.shapes[0].getProperties().outletid);
      }
    },
    loadOutlet: function (id) {
      let scope = this;
      this.outlets.forEach((outlet) => {
        if (outlet.id == id) {
          scope.previewOutlet.id = outlet.id;
          scope.previewOutlet.city = outlet.city;
          scope.previewOutlet.qualityScore = outlet.quality_Score;
          scope.previewOutlet.quantityScore = outlet.quantity_Score;
          scope.previewOutlet.phLevel = outlet.quality_Ph;
          scope.previewOutlet.hardness = outlet.quality_hardness;
          scope.previewOutlet.availableQuota = outlet.consumption_Quota_Available;
          scope.previewOutlet.consumedQuota = outlet.consumption_Quota;
        }
      });
    },
    reset: function () {
      this.outlets = [];
      this.loadOutlets();
    },
    selectCity: function (e, cityId) {
      this.selectedCityId = cityId;
      this.reset();
    },
    loadOutlets: function () {
      let scope = this;
      axios
        .get(
          "https://bhoojal-api.azurewebsites.net/api/" +
            scope.selectedCityId +
            "/outlets?code=6GMm7dASaC5HUzaEB/7xsZ35ShKMBh2azycF/FPG6TMZLB/FhEu0Jg=="
        )
        .then((response) => {
          console.log(response);
          scope.outlets = [];
          response.data.forEach((item) => {
            item.properties = {
              outletid: item.id,
            };
            scope.outlets.push(item);
          });
        })
        .catch((error) => {
          scope.errorMessage =
            "Outlet data for city " +
            scope.selectedCityId +
            " is not available.";
          console.log("Error", error);
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
    this.loadOutlets();
  },
};
</script>

<style scoped>
.AzureMapPopup {
  max-width: 200px;
  padding: 1rem;
}
</style>