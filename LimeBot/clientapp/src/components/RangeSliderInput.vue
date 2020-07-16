<template>
    <v-range-slider
        v-model="value"
        class="align-center"
        v-bind="$attrs"
        thumb-label
        :label="label" persistent-hint
    >
        <slot/>
    </v-range-slider>
</template>
<script>
import store from '@/store';
export default {
    inheritAttrs: false,
    props: {
        minId: String,
        maxId: String,
        label: String
    },
    computed: {
        value: {
            get() {
                return [store.state.settings[this.minId], store.state.settings[this.maxId]]
            },
            set(val) {
                store.commit('set', {[this.minId]: val[0], [this.maxId]: val[1]});
                this.$emit('input', val);
            }
        }
    },
    created() {
        this.$emit('input', this.value);
    }
}
</script>