import Vue from 'vue';
import Vuetify from 'vuetify';
import 'vuetify/dist/vuetify.min.css';
import { preset } from 'vue-cli-plugin-vuetify-preset-rally/preset';

Vue.use(Vuetify);

export default new Vuetify({
  preset,
  theme: {
    dark: true
  }
});
