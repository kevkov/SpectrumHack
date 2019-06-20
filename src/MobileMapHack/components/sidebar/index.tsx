import React from "react";
import {
    Content,
    Text,
    ListItem,
    Icon,
    Container,
    Left,
    Right,
    View, Badge
} from "native-base";
import {DrawerItemsProps} from "react-navigation";
import Constants from 'expo-constants';
import {FlatList} from "react-native";
import {Journey} from "../../domain/types";

const myJourneys:Journey[] = [
    {
        id: 1,
        name: "Home to Work",
        icon: "business",
        start: {
            latitude: 51.4511732, longitude: -0.2138706
        },
        startName: "Westminster",
        end: {
            latitude: 51.5250836, longitude: -0.0769465
        },
        endName: "North Greenwich"
    },
    {
        id: 2,
        name: "Work to Heathrow",
        icon: "business",
        start: {
            latitude: 51.4511731, longitude: -0.2138706
        },
        startName: "North Greenwich",
        end: {
            latitude: 51.5250836, longitude: -0.0769465
        },
        endName: "Heathrow"
    },
    {
        id: 3,
        name: "Mum's",
        icon: "home",
        start: {
            latitude: 51.4511731, longitude: -0.2138706
        },
        startName: "Westminster",
        end: {
            latitude: 51.5250836, longitude: -0.0769465
        },
        endName: "Ealing"
    },
];

export const SideBar = (props: DrawerItemsProps) => {
    return (
        <Container style={{paddingTop: Constants.statusBarHeight}}>
            <Content>
                <View style={{
                    alignSelf: 'center',
                    flexDirection: 'row',
                    alignItems: 'center',
                }}>
                    <Icon name={"md-person"}
                          style={
                              {
                                  fontSize: 96
                              }
                          }/>
                    <View style={{
                        marginLeft: 15
                    }}>
                        <Text>Jane</Text>
                        <Text>Public</Text>
                        <Badge success><Text>72</Text></Badge>
                    </View>
                </View>
                <FlatList
                    data={myJourneys}
                    keyExtractor={(item) => item.id.toString()}
                    renderItem={datum =>
                        <ListItem
                            button
                            noBorder
                            onPress={() => {
                                props.navigation.closeDrawer();
                                props.navigation.navigate("Route",
                                    {journey: datum.item});
                            }}
                        >
                            <Left>
                                <Icon
                                    active
                                    type={"MaterialIcons"}
                                    name={datum.item.icon}
                                    style={{color: "#777", fontSize: 26, width: 30}}
                                />
                                <Text>
                                    {datum.item.name}
                                </Text>
                            </Left>
                            <Right />
                        </ListItem>}
                />
            </Content>
        </Container>
    )
};
