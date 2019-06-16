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
import { Location} from "../../domain/types";
import styles from './styles';

const data =  [
    {
        name: "Home to Work",
        icon: "business",
        start: {
            latitude: 51.4511732, longitude: -0.2138706
        },
        end: {
            latitude: 51.5250836, longitude: -0.0769465
        }
    },
    {
        name: "Work to Heathrow",
        icon: "business",
        start: {
            latitude: 51.4511732, longitude: -0.2138706
        },
        end: {
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
                            onPress={() => props.navigation.navigate("Map",
                                {origin: datum.start, destination: datum.end})}>
                            <Left>
                                <Icon
                                    active
                                    type={"MaterialIcons"}
                                    name={datum.icon}
                                    style={{ color: "#777", fontSize: 26, width: 30 }}
                                />
                                <Text style={styles.text}>
                                    {datum.name}
                                </Text>
                            </Left>
                        </ListItem>}
                />
            </Content>
        </Container>
    )
};
