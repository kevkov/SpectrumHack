import React from "react";
import {
    Content,
    Text,
    List,
    ListItem,
    Icon,
    Container,
    Left,
    Right,
    Badge
} from "native-base";
import { DrawerItemsProps} from "react-navigation";
import { Constants } from 'expo';

export const SideBar = (props:DrawerItemsProps) => {
    return (
        <Container style={{paddingTop: Constants.statusBarHeight}}>
            <Content>
                <Text>Sidebar</Text>
            </Content>
        </Container>
    )
}
