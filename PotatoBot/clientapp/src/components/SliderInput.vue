<template>
    <v-slider
        v-model="value"
        class="align-center"
        v-bind="$attrs"
        thumb-label
        :label="label"
    >
        <slot/>
    </v-slider>
</template>
<script>
import store from '@/store';
export default {
    inheritAttrs: false,
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