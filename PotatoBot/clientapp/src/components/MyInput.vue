<template>
<v-text-field
    :id="id"
    :label="label"
    required
    width="200px"
    v-model="value"
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
                return store.state.settings[this.id];
            },
            set(val) {
                store.commit('set', {[this.id]: val});
            }
        }
    }
}
</script>