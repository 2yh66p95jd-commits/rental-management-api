import { createRouter, createWebHistory } from 'vue-router'

import DashboardView from '../views/DashboardView.vue'
import PropertiesView from '../views/PropertiesView.vue'
import TenantsView from '../views/TenantsView.vue'
import LeasesView from '../views/LeasesView.vue'
import PaymentsView from '../views/PaymentsView.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', component: DashboardView },
    { path: '/properties', component: PropertiesView },
    { path: '/tenants', component: TenantsView },
    { path: '/leases', component: LeasesView },
    { path: '/payments', component: PaymentsView },
  ],
})

export default router
