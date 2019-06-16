import React from "react";
import {
    Content,
    Text,
    ListItem,
    Icon,
    Container,
    Left,
    Right,
    Badge
} from "native-base";
import {DrawerItemsProps} from "react-navigation";
import {Constants} from 'expo';
import {Location} from "../../domain/types";
import styles from './styles';
import {FlatList} from "react-native";

const data = [
    {
        key: "Home to Work",
        icon: "business",
        start: {
            latitude: 51.4511732, longitude: -0.2138706
        },
        end: {
            latitude: 51.5250836, longitude: -0.0769465
        }
    },
    {
        key: "Work to Heathrow",
        icon: "business",
        start: {
            latitude: 51.4511731, longitude: -0.2138706
        },
        end: {
            latitude: 51.5250836, longitude: -0.0769465
        }
    },
];

export const SideBar = (props: DrawerItemsProps) => {
    return (
        <Container style={{paddingTop: Constants.statusBarHeight}}>
            <Content>
                <FlatList
                    data={data}
                    renderItem={datum =>
                        <ListItem
                            button
                            noBorder
                            onPress={() => {
                                props.navigation.navigate("Route",
                                    {origin: datum.item.start, destination: datum.item.end});
                                props.navigation.closeDrawer();
                            }}>
                            <Left>
                                <Icon
                                    active
                                    type={"MaterialIcons"}
                                    name={datum.item.icon}
                                    style={{color: "#777", fontSize: 26, width: 30}}
                                />
                                <Text style={styles.text}>
                                    {datum.item.key}
                                </Text>
                            </Left>
                        </ListItem>}
                />
            </Content>
        </Container>
    )
};
