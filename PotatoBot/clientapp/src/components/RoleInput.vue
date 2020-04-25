<template>
<v-autocomplete
    :label="label"
    width="200px"
    v-model="value"
    :items="items"
    outlined
    multiple
    chips small-chips deletable-chips
/>
</template>
<script>
import store from '@/store';
export default {
    props: {
        id: String,
        label: String
    },
    computed: {
        value: {
            get() {
                return store.state.settings[this.id] || [];

            },
            set(val) {
                store.commit('set', {[this.id]: val});
            }
        },
        items() {
            var roles = store.state.server.roles.map(i=>({text:i.name, value: i.id}));
            return roles;
        }
    }
}
</script>