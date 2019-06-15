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

const data =  [
    {
        "name": "Home to Work",
        "start": {
            latitude: 51.4511732, longitude: -0.2138706
        },
        "end": {
            latitude: 51.5250836, longitude: -0.0769465
        }
    },
    {
        "name": "Work to Heathrow",
        "start": {
            latitude: 51.4511732, longitude: -0.2138706
        },
        "end": {
            latitude: 51.5250836, longitude: -0.0769465
        }
    },
];

export const SideBar = (props:DrawerItemsProps) => {
    return (
        <Container style={{paddingTop: Constants.statusBarHeight}}>
            <Content>
                <List
                    dataArray={data}
                    renderRow={datum =>
                        <ListItem
                            button
                            noBorder
                            onPress={() => props.navigation.navigate("Map")}>
                            <Text>{datum.name}</Text>
                        </ListItem>}
                />
            </Content>
        </Container>
    )
}
