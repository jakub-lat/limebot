<template>
<v-switch
    :label="label"
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
                this.$emit('input', val);
            }
        }
    },
    created() {
        this.$emit('input', this.value);
    }
}
</script>