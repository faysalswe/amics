"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.navigation = void 0;
var componentType_1 = require("./pages/models/componentType");
exports.navigation = [
    {
        text: 'Home',
        path: '/home',
        icon: 'home',
    },
    {
        text: 'Dashboard',
        icon: 'folder',
        items: [
            {
                text: 'Designer',
                //path: '/profile',
                title: ' Designer',
                type: componentType_1.ComponentType.Designer
             },
            // {
            //     text: 'Report Viewer',
            //     path: '/reportviewer',
            //     title: 'Report Viewer',
            //     type: componentType_1.ComponentType.ReportViewer
            // },
        ]
    },
    {
        text: 'Inventory',
        icon: 'folder',
        items: [
            {
                text: 'Part Master & BOM',
                path: '/profile',
                title: 'Part Master',
                type: componentType_1.ComponentType.PartMaster
            },
            {
                text: 'Increase Inventory',
                path: '/increase-inventory',
                title: 'Increase Inventory',
                type: componentType_1.ComponentType.IncreaseInventory
            },
            {
                text: 'Decrease Inventory',
                path: '/decrease-inventory',
                title: 'Decrease Inventory',
                type: componentType_1.ComponentType.DecreaseInventory
            },
            // {
            //   text: 'Adjust Inventory',
            //   path: '/adjustInventory',
            //   title: 'Adjust Inventory',
            //   type: ComponentType.Profile
            // },
            {
                text: 'Change Location',
                path: '/changeLocation',
                title: 'Change Location',
                type: componentType_1.ComponentType.ChangeLocation
            },
            {
                text: 'Inquiry',
                path: '/inquiry',
                title: 'Inquiry',
                type: componentType_1.ComponentType.Inquiry
            },
            // {
            //   text: 'Serial Documents',
            //   path: '/serialDocuments',
            //   title: 'Serial Documents',
            //   type: ComponentType.SerialDocuments
            // },
            {
                text: 'Change Serial',
                path: '/changeSerial',
                title: 'Change Serial',
                type: componentType_1.ComponentType.ChangeSerial
            },
            {
                text: 'Reports',
                path: '/reports',
                title: 'Reports',
                type: componentType_1.ComponentType.Reports
            },
            {
                text: 'Bulk Transfer',
                path: '/bulkTransfer',
                title: 'Bulk Transfer',
                type: componentType_1.ComponentType.BulkTransfer
            },
        ]
    },
    {
        text: 'Sales Order',
        icon: 'folder',
        items: [
            {
                text: 'MDAT',
                path: '/mdat',
                title: 'mdat',
                type: componentType_1.ComponentType.Mdat
            },
            {
                text: 'Equipment Request',
                path: '/equipment',
              title: 'Equipment Request',
                type: componentType_1.ComponentType.ER
            },
            {
                text: 'Shipment',
                path: '/shipment',
                title: 'Shipment',
                type: componentType_1.ComponentType.Shipment
            },
            {
                text: 'Reports',
                path: '/report2',
                title: 'Reports',
                type: componentType_1.ComponentType.Report2
            },
        ]
    }
];
//# sourceMappingURL=app-navigation.js.map
