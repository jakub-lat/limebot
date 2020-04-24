import Vue from 'vue';
import App from './App.vue';
import router from './router';
import vuetify from './plugins/vuetify';
import VueCookies from 'vue-cookies';
import api from './plugins/api';

Vue.config.productionTip = false;

Vue.use(VueCookies);
Vue.$cookies.config('7d');

Vue.prototype.$api = Vue.$api = api;

new Vue({
  router,
  vuetify,
  render: h => h(App)
}).$mount('#app');