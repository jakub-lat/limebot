<template>
<v-textarea v-if="textarea"
    :label="label"
    :required="required"
    :disabled="disabled"
    :maxlength="maxlength"
    :counter="!!maxlength"
    v-bind="$attrs"
    width="200px"
    v-model="value"
    outlined no-resize
    />
<v-text-field v-else
    :label="label"
    :required="required"
    :disabled="disabled"
    :maxlength="maxlength"
    :counter="!!maxlength"
    v-bind="$attrs"
    width="200px"
    v-model="value"
    outlined
    />
</template>
<script>
import store from '@/store';
export default {
    inheritAttrs: false,
    props: {
        id: String,
        label: String,
        required: Boolean,
        disabled: Boolean,
        textarea: Boolean,
        maxlength: String
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