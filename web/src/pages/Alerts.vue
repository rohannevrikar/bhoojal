<template>
  <div class="content">
    <div class="container-fluid">
      <card>
        <template slot="header">
          <h4 class="card-title">Alerts</h4>
          <p class="card-category">
            Alerts from various outlets being monitored
          </p>
          <br />
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
        </template>
        <div class="card-body">
          <div v-if="alerts == undefined" class="alert alert-info alert-with-icon" data-notify="container">
            <span data-notify="icon" class="nc-icon nc-satisfied"></span>
            <span data-notify="message">Hurray! No Alerts.</span>
          </div>
          <ul style="list-style-type: none">
            <li v-for="alert in alerts" :key="alert.outletId">
              <card>
                <div class="col-5">
                  <div class="icon-big">
                    <h4 class="card-title">
                      <i class="nc-icon nc-notification-70 text-warning"></i>
                      Alert for outlet {{ alert.outletId }}
                    </h4>
                  </div>
                </div>

                <p class="card-category"></p>
                <div class="card-body">
                  <slot>
                    <ul id="test-next" style="list-style-type: none">
                      <template v-for="item in alert.alert.metrics">
                        <li v-if="alert.alert.isAlert" :key="item.value">
                          <span v-if="item.metric === 'consumption'">
                            Consumption exceeded the assigned quota. Consumed:
                            {{ item.value }} threshold: {{ item.threshold }}
                          </span>
                          <span v-if="item.metric === 'hardness'">
                            Water hardness exceeded the threshold. Current
                            hardness:
                            {{ item.value }} threshold: {{ item.threshold }}
                          </span>
                          <span v-if="item.metric === 'phLevelHigh'">
                            Water pH level exceeded the max threshold. Current
                            pH level:
                            {{ item.value }} threshold: {{ item.threshold }}
                          </span>
                          <span v-if="item.metric === 'phLevelLow'">
                            Water pH level is below the minimum threshold.
                            Current pH level:
                            {{ item.value }} threshold: {{ item.threshold }}
                          </span>
                        </li>
                      </template>
                    </ul>
                  </slot>
                </div>
              </card>
            </li>
          </ul>
        </div>
      </card>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import Card from "src/components/Cards/Card.vue";

export default {
  components: {},
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
      alerts: [],
    };
  },
  methods: {
    reset: function () {
      this.alerts = [];
      this.loadAlerts();
    },
    loadAlerts: function () {
      console.log("Loading alerts for city");
      let scope = this;
      axios
        .get(
          "https://bhoojal-alerts-api.azurewebsites.net/api/GetAlertsByCity?city=" +
            scope.selectedCityId
        )
        .then((response) => {
          console.log(response);
          scope.alerts = response.data[0];
        });
    },
    selectCity: function (e, cityId) {
      this.selectedCityId = cityId;
      this.reset();
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
    this.loadAlerts();
  },
};
</script>