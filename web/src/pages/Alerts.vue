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
          <ul style="list-style-type:none;">
            <li v-for="alert in alerts" :key="alert.outletId">
              <card>
                <h4 class="card-title">Alert for outlet {{ alert.outletId }}</h4>
                <p class="card-category"></p>
                <div class="card-body">
                  <slot>
                    <ul id="test-next" style="list-style-type:none;">
                      <template v-for="item in alert.alert.metrics">
                        <li v-if="alert.alert.isAlert" :key="item.value">
                          Consumption exceeded the assigned quota. Consumed:
                          {{ item.value }} threshold: {{ item.threshold }}
                        </li>
                      </template>
                    </ul>
                  </slot>
                </div>
              </card>
            </li>
          </ul>
        </template>
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
    loadAlerts: function () {
      console.log("Loading alerts for city");
      let scope = this;
      axios
        .get("https://bhoojal-alerts-api.azurewebsites.net/api/alerts/8")
        .then((response) => {
          console.log(response);
          scope.alerts = response.data;
        });
    },
  },
  mounted() {
    console.log("Loading alerts for city");
      let scope = this;
      axios
        .get("https://bhoojal-alerts-api.azurewebsites.net/api/GetAlertsByCity?city=pcmc")
        .then((response) => {
          console.log(response);
          scope.alerts = response.data[0];
        });
  },
};
</script>