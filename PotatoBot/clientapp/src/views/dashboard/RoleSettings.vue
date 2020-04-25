<template>
    <v-container>
        <h1 class="display-1 mb-3">Role utilites</h1>
        <v-form>
            <v-row>
                <v-col md="5">
                    <role-input id="autoRolesOnJoin" label="Roles to give when a member joins"/>
                    
                </v-col>
            </v-row>
        </v-form>
    </v-container>
</template>
<script>
import RoleInput from '@/components/RoleInput.vue';
export default {
    components: {RoleInput}
}
</script>