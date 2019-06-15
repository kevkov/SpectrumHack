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
import {DrawerItemsProps} from "react-navigation";

export const SideBar = (props:DrawerItemsProps) => {
    return (
        <Container>
            <Content>
                <Text>Sidebar</Text>
            </Content>
        </Container>
    )
}
