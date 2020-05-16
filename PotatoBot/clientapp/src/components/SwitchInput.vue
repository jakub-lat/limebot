<template>
<v-switch
    :label="label"
    width="200px"
    v-model="value"
    v-bind="$attrs" persistent-hint
    />
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
                store.commit('set', {[this.id]: !!val});
                this.$emit('input', val);
            }
        }
    },
    created() {
        this.$emit('input', this.value);
    }
}
</script>